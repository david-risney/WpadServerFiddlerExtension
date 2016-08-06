using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;

namespace WpadFiddlerExtension
{
    class Log
    {
        private static Log sLog = new Log();
        public static Log Get() { return sLog; }

        public struct Entry
        {
            public enum EntryType
            {
                Note,
                Error
            };

            public Entry(EntryType type, String text, object context)
            {
                mType = type;
                mText = text;
                mTime = DateTime.Now;
                mContext = context;
            }

            private EntryType mType;
            private String mText;
            private DateTime mTime;
            private object mContext;

            public EntryType Type { get { return mType; } }
            public String Text { get { return mText; } }
            public DateTime Time { get { return mTime; } }
            public object Context { get { return mContext; } }
        };

        private List<Entry> mLog = new List<Entry>();

        public Log()
        {
        }

        public void AddLogEntry(Entry.EntryType logType, String logText, object context)
        {
            Entry entry = new Entry(logType, logText, context);
            ThreadPool.QueueUserWorkItem(new WaitCallback(AddLogEntryThreadProc), entry);
        }

        private void _AddLogEntry(Entry entry)
        {
            mLog.Add(entry);
            Fiddler.FiddlerApplication.Log.LogString("WPAD DHCP Server: " + entry.Text);
            if (OnNewEntry != null)
                OnNewEntry(entry);
        }

        public static void AddLogEntryThreadProc(object o)
        {
            Entry entry = (Entry)o;
            Log.Get()._AddLogEntry(entry);
        }

        public void AddLogEntry(Entry.EntryType logType, String logText)
        {
            AddLogEntry(logType, logText, null);
        }

        public ReadOnlyCollection<Entry> Logs { get { return mLog.AsReadOnly(); } }

        public delegate void NewEntry(Entry entry);
        public event NewEntry OnNewEntry;
    }
}