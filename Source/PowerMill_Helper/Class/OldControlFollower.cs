using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

public class ControlFollower
{
    private readonly string controlTitle;
    private readonly FrameworkElement targetElement;
    private readonly Window hostWindow;
    private readonly DispatcherTimer timer;
    private readonly IntPtr mainWindowHandle;

    public ControlFollower(IntPtr mainWindow, string targetControlTitle, FrameworkElement elementToMove)
    {
        mainWindowHandle = mainWindow;
        controlTitle = targetControlTitle;
        targetElement = elementToMove;
        hostWindow = Window.GetWindow(elementToMove);

        timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(10)
        };
        timer.Tick += Timer_Tick;
    }

    public void Start() => timer.Start();
    public void Stop() => timer.Stop();

    private void Timer_Tick(object sender, EventArgs e)
    {
        IntPtr hwndTarget = FindChildWindowByTitle(mainWindowHandle, controlTitle);
        if (hwndTarget == IntPtr.Zero)
            return;

        if (!GetWindowRect(hwndTarget, out RECT rect))
            return;

        Point topLeft = hostWindow.PointFromScreen(new Point(rect.Left, rect.Top));
        targetElement.Margin = new Thickness(topLeft.X, topLeft.Y, 0, 0);
    }

    #region 查找指定标题控件
    private IntPtr FindChildWindowByTitle(IntPtr parent, string title)
    {
        IntPtr child = IntPtr.Zero;
        while ((child = FindWindowEx(parent, child, null, null)) != IntPtr.Zero)
        {
            StringBuilder sb = new StringBuilder(256);
            GetWindowText(child, sb, sb.Capacity);
            string windowText = sb.ToString();
            if (windowText == title)
                return child;

            // 递归查找子孙控件
            IntPtr grandChild = FindChildWindowByTitle(child, title);
            if (grandChild != IntPtr.Zero)
                return grandChild;
        }
        return IntPtr.Zero;
    }
    #endregion

    #region Win32 API
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left, Top, Right, Bottom;
    }
    #endregion
}
