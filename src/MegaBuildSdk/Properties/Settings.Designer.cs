﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MegaBuild.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>\:\s*Error\s</string>
  <string>\:\s*fatal error\s</string>
  <string>\:\sINTERNAL COMPILER ERROR\s</string>
  <string>\:\sCommand line error\s</string>
  <string>^\s*Error\s*\:</string>
  <string>^\s*Failed\s</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection ExecutableStep_ErrorRegexs {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["ExecutableStep_ErrorRegexs"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>\:\s*Warning\s</string>
  <string>\:\s*Command line warning\s</string>
  <string>^\s*Warning\:</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection ExecutableStep_WarningRegexs {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["ExecutableStep_WarningRegexs"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4000")]
        public ushort ExecutableStep_KillTimeoutMilliseconds {
            get {
                return ((ushort)(this["ExecutableStep_KillTimeoutMilliseconds"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1000")]
        public ushort Project_FinishBuildSleepMilliseconds {
            get {
                return ((ushort)(this["Project_FinishBuildSleepMilliseconds"]));
            }
        }
    }
}
