using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerMill_Helper
{
     class WindowTracker
    {
        private IntPtr _hook;
        private IntPtr _targetHwnd;
        private IntPtr _lastScreenHandle;

        public WindowTracker(IntPtr targetWindow)
        {
            _targetHwnd = targetWindow;
        _lastScreenHandle = GetScreenHandle(_targetHwnd);

        _hook = SetWinEventHook(
            EVENT_OBJECT_LOCATIONCHANGE,
            EVENT_OBJECT_LOCATIONCHANGE,
            IntPtr.Zero,
            _callback,
            0,
            0,
            WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);
    }

    private WinEventDelegate _callback = (hWinEventHook, eventType, hwnd, idObject, idChild, dwEventThread, dwmsEventTime) =>
    {
        if (hwnd == IntPtr.Zero || hwnd != _instance._targetHwnd) return;

        IntPtr currentScreen = GetScreenHandle(hwnd);
        if (currentScreen != _instance._lastScreenHandle)
        {
            _instance._lastScreenHandle = currentScreen;
            _instance.OnScreenChanged(currentScreen);
        }
    };

        private static WindowTracker _instance;
        public static WindowTracker Start(IntPtr hwnd)
        {
            _instance = new WindowTracker(hwnd);
            return _instance;
        }

        public Action<IntPtr> OnScreenChanged = delegate { };

        public void Stop()
        {
            UnhookWinEvent(_hook);
        }

        // 获取窗口在哪个屏幕上
        private static IntPtr GetScreenHandle(IntPtr hwnd)
        {
            return MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
        }

    // Win32 definitions
    private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
        IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    private const uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B;
    private const uint WINEVENT_OUTOFCONTEXT = 0x0000;
    private const uint WINEVENT_SKIPOWNPROCESS = 0x0002;

    [DllImport("user32.dll")]
    private static extern IntPtr SetWinEventHook(
        uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc,
        uint idProcess, uint idThread, uint dwFlags);

    [DllImport("user32.dll")]
    private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

    private const uint MONITOR_DEFAULTTONEAREST = 2;
}
}
