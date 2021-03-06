﻿using System;
using System.Text;
using System.Runtime.InteropServices;


namespace Tsunami.Core
{
    using AddTorrentParamsHandle = HandleRef;
    using TorrentInfoHandle = HandleRef;


    public class AddTorrentParams : IDisposable
    {
        #region PInvoke
        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr AddTorrentParams_Create();

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddTorrentParams_Destroy(AddTorrentParamsHandle h);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddTorrentParams_Name_Set(AddTorrentParamsHandle handle, string name);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddTorrentParams_Name_Get(AddTorrentParamsHandle handle, StringBuilder str, int size);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddTorrentParams_SavePath_Set(AddTorrentParamsHandle handle, string name);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddTorrentParams_SavePath_Get(AddTorrentParamsHandle handle, StringBuilder str, int size);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr AddTorrentParams_TorrentInfo_Get(AddTorrentParamsHandle handle);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddTorrentParams_TorrentInfo_Set(AddTorrentParamsHandle handle, TorrentInfoHandle ti);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddTorrentParams_Flags_Set(AddTorrentParamsHandle handle, uint flags);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern uint AddTorrentParams_Flags_Get(AddTorrentParamsHandle handle);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddTorrentParams_Url_Set(AddTorrentParamsHandle handle, string name);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddTorrentParams_Url_Get(AddTorrentParamsHandle handle, StringBuilder str, int size);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern uint AddTorrentParams_Trackers_Size_Get(AddTorrentParamsHandle handle);

        [DllImport("TsunamiBridge", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddTorrentParams_Trackers_Get(AddTorrentParamsHandle handle, [In][Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]string[] trk, uint size);

        #endregion PInvoke

        AddTorrentParamsHandle handle;
        private StringBuilder sb = new StringBuilder(256);

        private ATPFlags flags;
        public ATPFlags Flags
        {
            get
            {
                flags = (ATPFlags)AddTorrentParams_Flags_Get(handle);
                return flags;
            }
            set
            {
                flags = value;
                AddTorrentParams_Flags_Set(handle, (uint)value);
            }
        }

        public TorrentInfo ti
        {
            get
            {
                ti.Handle = new HandleRef(this,AddTorrentParams_TorrentInfo_Get(handle));
                return ti;
            }
            set
            {
                AddTorrentParams_TorrentInfo_Set(handle, value.Handle);
            }
        }

        private string name;
        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(name))
                {
                    AddTorrentParams_Name_Get(handle, sb, sb.Capacity);
                    name = sb.ToString();
                }
                return name;
            }
            set
            {
                name = value;
                AddTorrentParams_Name_Set(handle, value);
            }
        }

        private string url;
        public string Url
        {
            get
            {
                if (String.IsNullOrEmpty(url))
                {
                    AddTorrentParams_Url_Get(handle, sb, sb.Capacity);
                    url = sb.ToString();
                }
                return url;
            }
            set
            {
                url = value;
                AddTorrentParams_Url_Set(handle, value);
            }
        }

        private string[] trackers;
        public string[] Trackers
        {
            get
            {
                if (trackers == null)
                {
                    uint size = AddTorrentParams_Trackers_Size_Get(handle);
                    if (size != 0)
                    {
                        trackers = new string[size];
                        AddTorrentParams_Trackers_Get(handle, trackers, size);
                    }
                }

                return trackers;
            }
            set
            {
                trackers = value;
            }
        }

        private string save_path;
        public string SavePath
        {
            get
            {
                if (String.IsNullOrEmpty(save_path))
                {
                    AddTorrentParams_SavePath_Get(handle, sb, sb.Capacity);
                    save_path = sb.ToString();
                }
                return save_path;
            }
            set
            {
                save_path = value;
                AddTorrentParams_SavePath_Set(handle, value);
            }
        }

        public AddTorrentParams()
        {
            IntPtr h = AddTorrentParams_Create();
            handle = new HandleRef(this, h);
        }


        public AddTorrentParamsHandle Handle
        {
            get
            {
                return handle;
            }
        }

        public void Dispose()
        {
            CleanUp();
            GC.SuppressFinalize(this);
        }

        private void CleanUp()
        {
            AddTorrentParams_Destroy(handle);
            handle = new HandleRef(this, IntPtr.Zero);
        }

        ~AddTorrentParams()
        {
            CleanUp();
        }

    }
}
