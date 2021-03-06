﻿using BluePointLilac.Methods;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ContextMenuManager
{
    static class AppConfig
    {
        static AppConfig()
        {
            foreach(string dirPath in new[] { ConfigDir, ProgramsDir, BackupDir, LangsDir, DicsDir, WebDicsDir, UserDicsDir })
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        [DllImport("kernel32.dll")]
        private static extern ushort GetUserDefaultUILanguage();

        public static readonly string AppConfigDir = $@"{Application.StartupPath}\Config";
        public static readonly string AppDataConfigDir = Environment.ExpandEnvironmentVariables(@"%AppData%\ContextMenuManager\Config");
        public static readonly string ConfigDir = Directory.Exists(AppConfigDir) ? AppConfigDir : AppDataConfigDir;
        public static readonly bool SaveToAppDir = ConfigDir == AppConfigDir;
        public static string ConfigIni = $@"{ConfigDir}\Config.ini";
        public static string BackupDir = $@"{ConfigDir}\Backup";
        public static string LangsDir = $@"{ConfigDir}\Languages";
        public static string ProgramsDir = $@"{ConfigDir}\Programs";
        public static string DicsDir = $@"{ConfigDir}\Dictionaries";
        public static string WebDicsDir = $@"{DicsDir}\Web";
        public static string UserDicsDir = $@"{DicsDir}\User";
        public static string WebGuidInfosDic = $@"{WebDicsDir}\{GUIDINFOSDICINI}";
        public static string UserGuidInfosDic = $@"{UserDicsDir}\{GUIDINFOSDICINI}";
        public static string WebThirdRulesDic = $@"{WebDicsDir}\{ThIRDRULESDICXML}";
        public static string UserThirdRulesDic = $@"{UserDicsDir}\{ThIRDRULESDICXML}";
        public static string WebEnhanceMenusDic = $@"{WebDicsDir}\{ENHANCEMENUSICXML}";
        public static string UserEnhanceMenusDic = $@"{UserDicsDir}\{ENHANCEMENUSICXML}";
        public static string HashLnkExePath = $@"{ProgramsDir}\HashLnk.exe";
        public const string ZH_CNINI = "zh-CN.ini";
        public const string GUIDINFOSDICINI = "GuidInfosDic.ini";
        public const string ThIRDRULESDICXML = "ThirdRulesDic.xml";
        public const string ENHANCEMENUSICXML = "EnhanceMenusDic.xml";

        public static readonly string[] EngineUrls =
        {
            "https://www.baidu.com/s?wd=%s",          //百度搜索
            "https://www.bing.com/search?q=%s",       //必应搜索
            "https://www.google.com/search?q=%s",     //谷歌搜索
            "https://www.dogedoge.com/results?q=%s",  //多吉搜索
            "https://www.sogou.com/web?query=%s",     //搜狗搜索
            "https://www.so.com/s?q=%s",              //360搜索
        };

        private static readonly IniWriter ConfigWriter = new IniWriter(ConfigIni);

        public static string LanguageIniPath => $@"{LangsDir}\{Language}.ini";

        public static string Language
        {
            get
            {
                string language = ConfigWriter.GetValue("General", "Language");
                if(language == string.Empty)
                {
                    language = new CultureInfo(GetUserDefaultUILanguage()).Name;
                }
                if(!File.Exists($@"{LangsDir}\{language}.ini"))
                {
                    language = string.Empty;
                }
                return language;
            }
            set => ConfigWriter.SetValue("General", "Language", value);
        }

        public static bool AutoBackup
        {
            get => ConfigWriter.GetValue("General", "AutoBackup") != "0";
            set => ConfigWriter.SetValue("General", "AutoBackup", (value ? 1 : 0).ToString());
        }

        public static DateTime LastCheckUpdateTime
        {
            get
            {
                try
                {
                    string time = ConfigWriter.GetValue("General", "LastCheckUpdateTime");
                    //二进制数据时间不会受系统时间格式影响
                    return DateTime.FromBinary(Convert.ToInt64(time));
                }
                catch
                {
                    //将上次检测更新时间推前到两个月前
                    return DateTime.Today.AddMonths(-2);
                }
            }
            set
            {
                ConfigWriter.SetValue("General", "LastCheckUpdateTime", value.ToBinary().ToString());
            }
        }

        public static bool ProtectOpenItem
        {
            get => ConfigWriter.GetValue("General", "ProtectOpenItem") != "0";
            set => ConfigWriter.SetValue("General", "ProtectOpenItem", (value ? 1 : 0).ToString());
        }

        public static string EngineUrl
        {
            get
            {
                string url = ConfigWriter.GetValue("General", "EngineUrl");
                if(url.IsNullOrWhiteSpace()) url = EngineUrls[0];
                return url;
            }
            set
            {
                ConfigWriter.SetValue("General", "EngineUrl", value);
            }
        }

        public static bool ShowFilePath
        {
            get => ConfigWriter.GetValue("General", "ShowFilePath") == "1";
            set => ConfigWriter.SetValue("General", "ShowFilePath", (value ? 1 : 0).ToString());
        }

        public static bool WinXSortable
        {
            get => ConfigWriter.GetValue("General", "WinXSortable") == "1";
            set => ConfigWriter.SetValue("General", "WinXSortable", (value ? 1 : 0).ToString());
        }

        public static bool OpenMoreRegedit
        {
            get => ConfigWriter.GetValue("General", "OpenMoreRegedit") == "1";
            set => ConfigWriter.SetValue("General", "OpenMoreRegedit", (value ? 1 : 0).ToString());
        }
    }
}