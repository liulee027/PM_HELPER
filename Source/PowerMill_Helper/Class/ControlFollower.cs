using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

public class SmoothControlFollower
{
    private readonly string controlTitle;
    private readonly IntPtr mainWindowHandle;
    private readonly FrameworkElement targetElement;
    private readonly Window hostWindow;

    private readonly TranslateTransform transform = new TranslateTransform();
    private readonly DispatcherTimer timer;

    private double currentX = 0;
    private double currentY = 0;

    public SmoothControlFollower(IntPtr mainHwnd, string title, FrameworkElement elementToMove)
    {
        mainWindowHandle = mainHwnd;
        controlTitle = title;
        targetElement = elementToMove;
        hostWindow = Window.GetWindow(elementToMove);

        targetElement.RenderTransform = transform;

        timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // ~60fps
        };
        timer.Tick += Timer_Tick;
    }

    public void Start() => timer.Start();
    public void Stop() => timer.Stop();

    private void Timer_Tick(object sender, EventArgs e)
    {
        IntPtr hwnd = FindChildWindowByTitle(mainWindowHandle, controlTitle);
        if (hwnd == IntPtr.Zero || !GetWindowRect(hwnd, out RECT rect)) return;

        // Convert screen coordinates to WPF window coordinates
        Point topLeft = hostWindow.PointFromScreen(new Point(rect.Left, rect.Top));

        // 插值过渡：使移动过程平滑
        double lerpSpeed = 0.25;
        currentX = Lerp(currentX, topLeft.X, lerpSpeed);
        currentY = Lerp(currentY, topLeft.Y, lerpSpeed);

        transform.X = currentX;
        transform.Y = currentY;
    }

    private double Lerp(double from, double to, double speed)
    {
        return from + (to - from) * speed;
    }

    #region 查找控件
    private IntPtr FindChildWindowByTitle(IntPtr parent, string title)
    {
        IntPtr child = IntPtr.Zero;
        while ((child = FindWindowEx(parent, child, null, null)) != IntPtr.Zero)
        {
            StringBuilder sb = new StringBuilder(256);
            GetWindowText(child, sb, sb.Capacity);
            if (sb.ToString() == title)
                return child;

            IntPtr grandChild = FindChildWindowByTitle(child, title);
            if (grandChild != IntPtr.Zero)
                return grandChild;
        }
        return IntPtr.Zero;
    }
    #endregion

    #region Win32 API
    [DllImport("user32.dll")]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left, Top, Right, Bottom;
    }
    #endregion
}
