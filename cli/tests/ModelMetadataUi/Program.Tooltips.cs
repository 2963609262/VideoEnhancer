using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows.Forms;

partial class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    static extern IntPtr CreateDesktopW(string name, IntPtr device, IntPtr mode, uint flags, uint access, IntPtr security);
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetThreadDesktop(IntPtr desktop);

    static void UseTestDesktop()
    {
        // 独立测试桌面不切到前台，不移动用户鼠标，不启动3FUI。
        var desktop = CreateDesktopW("VideoEnhancerTooltips-" + Guid.NewGuid().ToString("N"), IntPtr.Zero, IntPtr.Zero, 0, 0x01FF, IntPtr.Zero);
        if (desktop == IntPtr.Zero || !SetThreadDesktop(desktop))
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
    }

    static void RunTooltipChecks(Assembly plugin)
    {
        var lake = Assembly.Load("LakeUI");
        var menuType = lake.GetType("LakeUI.ModernContextMenu")!;
        var itemType = menuType.GetNestedType("ModernMenuItem")!;
        var controllerType = plugin.GetType("videoenhancer.PluginPanel")!.GetNestedType("ModelMenuToolTipController", Flags)!;
        for (var pass = 1; pass <= 3; pass++)
        {
            var menu = Activator.CreateInstance(menuType)!;
            var item = Activator.CreateInstance(itemType, "模型介绍测试")!;
            ((IList)Get(menu, "Items")).Add(item);
            Set(menu, "AnimationDuration", 0);
            menuType.GetMethod("Show", new[] { typeof(int), typeof(int) })!.Invoke(menu, new object[] { 100, 100 });
            Application.DoEvents();
            using var popup = (Form)Field(menu, "当前弹出窗口");
            var tips = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(itemType, typeof(string)))!;
            tips.Add(item, "动画线稿修复；原生 4x");
            var controller = Activator.CreateInstance(controllerType, menu, tips)!;
            Call(controller, "Start");
            Call(controller, "ShowTip", popup, new Rectangle(0, 0, 100, 30), item, "动画线稿修复；原生 4x");
            var first = (Form)Field(controller, "_tipForm");
            Check(first.Visible && !first.IsDisposed, $"第{pass}次打开菜单显示介绍");
            first.Close();
            Check(first.IsDisposed, "模拟提示窗自行关闭并释放");
            Call(controller, "ShowTip", popup, new Rectangle(0, 0, 100, 30), item, "动画线稿修复；原生 4x");
            var second = (Form)Field(controller, "_tipForm");
            Check(second.Visible && !second.IsDisposed && !ReferenceEquals(first, second), "同一菜单重新创建已释放提示窗");
            using var submenuOwner = new Form { Size = new Size(120, 100), Location = new Point(250, 100) };
            submenuOwner.Show();
            Application.DoEvents();
            Call(controller, "ShowTip", submenuOwner, new Rectangle(0, 0, 100, 30), item, "另一分组模型");
            Check(ReferenceEquals(Field(controller, "_tipOwner"), submenuOwner) && ((Form)Field(controller, "_tipForm")).Visible,
                  "切换分组后提示窗关联当前弹窗");
            Call(controller, "Close");
            Check(((Form)Field(controller, "_tipForm")).IsDisposed, "菜单关闭释放提示窗");
            Call(menu, "Close");
            Application.DoEvents();
        }
        var provider = plugin.GetType("videoenhancer.ModelDescriptionProvider")!;
        var modelType = plugin.GetType("videoenhancer.ModelCatalogItem")!;
        string Description(string id, string architecture = "Compact", bool interpolation = false)
        {
            var model = Activator.CreateInstance(modelType)!;
            Set(model, "Id", id);
            Set(model, "DisplayName", id[(id.LastIndexOf('/') + 1)..]);
            Set(model, "Architecture", architecture);
            Set(model, "Scale", 2);
            return (string)provider.GetMethod("ModelIntroduction", Flags)!.Invoke(null, new[] { model, (object)interpolation })!;
        }
        Check(Description("PTH/OpenProteus-Compact-i2-2x").Contains("实拍"), "OpenProteus明确实拍用途");
        Check(Description("PTH/AniSD-DB-i2-SPAN-1x").Contains("色渗"), "AniSD DB说明去色渗");
        Check(Description("PTH/AniSD-PS-G6i2-2x").Contains("纯放大"), "AniSD PS说明纯放大");
        Check(Description("PTH/AniScale2-DITN-2x", "DITN").Contains("不推荐"), "DITN保留作者的限制说明");
        Check(Description("PTH/Nomos8k-span-otf-strong").Contains("照片"), "Nomos8k明确照片用途");
        Check(Description("PTH/unknown-model", "ESRGAN").Contains("未核实"), "未知权重不按架构猜训练题材");
        Check(Description("GIMM/gimm-f-lpips", "GIMM", true).Contains("FlowFormer"), "GIMM F解释光流差异");
        var results = new List<object>();
        using var catalog = JsonDocument.Parse(File.ReadAllText("cli/model-capabilities.json", System.Text.Encoding.UTF8));
        foreach (var entry in catalog.RootElement.GetProperty("models").EnumerateArray())
        {
            var id = entry.GetProperty("model").GetString()!;
            var text = Description(id, entry.GetProperty("architecture").GetString()!);
            if (text.Length == 0 || text.Length > 180) throw new Exception("介绍长度不合适：" + id);
            results.Add(new { id, introduction = text });
        }
        Directory.CreateDirectory("Artifacts/model-audit");
        File.WriteAllText("Artifacts/model-audit/model-introductions.json", JsonSerializer.Serialize(results,
            new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), System.Text.Encoding.UTF8);
        Check(results.Count == 98, "98项内置介绍简要且非空");
        Console.WriteLine("菜单提示生命周期和文案验证通过；仅使用独立测试桌面");
    }
}
