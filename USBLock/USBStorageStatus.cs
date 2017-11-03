using System;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Diagnostics;
using ProcessPrivileges;
using System.Security.Principal;
using System.IO; 
namespace USBLock
{
    public class USBStorageStatus
    {

        /**
        * return status of USB removables devices, using registry key HKLM\SYSTEM\CurrentControlSet\Services\UsbStor
        * Note: in case when usbstore drivers is readable by current users security context and SYSTEM account plugging 
        * in unknown USB storage device will automatically enable USB removable storage  
        */
        public bool getUSBStorageStatus()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\UsbStor"))
            {
                if (key == null)
                {
                    return false;
                }
                object tt = key.GetValue("Start");
                string status = tt.ToString();
                if (status == "4")
                {
                    return false;
                } else
                {
                    return true;
                }
            }
        }

        /**
        * Update driver file permissions  
        */
        private void updatePermissions(bool nextStatus)
        {
            foreach (string fname in new string[] { "inf\\usbstor.inf", "inf\\usbstor.pnf" }) {
                string path = Path.Combine(Environment.ExpandEnvironmentVariables("%systemroot%"), fname);
                using (new ProcessPrivileges.PrivilegeEnabler(Process.GetCurrentProcess(), Privilege.TakeOwnership))
                {
                    FileSecurity fSecurity = File.GetAccessControl(path);
                    fSecurity.SetOwner(WindowsIdentity.GetCurrent().User);
                    File.SetAccessControl(path, fSecurity);
                    fSecurity.SetAccessRuleProtection(true, false);
                    AuthorizationRuleCollection rules = fSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

                    foreach (FileSystemAccessRule rule in rules)
                    {
                        if (rule.IdentityReference.ToString().EndsWith("SYSTEM") || rule.IdentityReference.ToString().EndsWith("Users")) { // FIXME: use well known SID instead of group name.
                            fSecurity.RemoveAccessRule(rule);
                        }
                    }
                    AccessControlType permissionType;
                    if (nextStatus == true)
                    {
                        permissionType = AccessControlType.Allow;
                    } else
                    {
                        permissionType = AccessControlType.Deny;

                    }
                    fSecurity.AddAccessRule(new FileSystemAccessRule("USERS", FileSystemRights.ReadAndExecute, permissionType));
                    fSecurity.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, permissionType));
                    File.SetAccessControl(path, fSecurity);
                }
            }


        }
        /**
        * update registry entries 
        */
        private void updateRegistry(bool nextStatus)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\UsbStor", true))
            {
                if (nextStatus == true)
                {
                    key.SetValue("Start", 3, RegistryValueKind.DWord);
                }
                else
                {
                    key.SetValue("Start", 4, RegistryValueKind.DWord);

                }
            }
        }
        /**
        * Toggle USB removable device status 
        */ 
        public void toggleStatus(bool nextStatus)
        {
            this.updateRegistry(nextStatus);
            this.updatePermissions(nextStatus);


        }
    }
}
