#### Instructions for installing and uninstalling the BankID MFA Adapter ####
```

# Certificates
Install client certificate and allow adfs service user rights on the private key
Install root certificate.
Certificates for testing provided in the test project.

# EventLog
Should be created during install, otherwise run:
New-EventLog -LogName "Application" -Source "BankID"

# Config
Url to the service cant contain a slash at the end, breaks json
Certificate password not needed, as the certificates should be installed
RequirementCertificate is different between environments. For test: "1.2.3.4.25", production: "1.2.752.78.1.5"
Choose lookup method for civicnumber "UserLookupMethod": "SQL" or "UserLookupMethod": "LDAP"
For sql, multiple connections are available

# Copy files to ADFS
Copy files to ADFS server (c:\admin\install\<BankIDSecondFactor>) from ADFSBankIDSecondFactor
bankid.png						\images\bankid.png
BankIDSecondFactorMerged.dll	\bin\<configuration>\BankIDSecondFactorMerged.dll
BankIDSettings.json		
Newtonsoft.json.dll		
Install-ADFSBankIDAdapter.ps1	\scripts\
Uninstall-ADFSBankIDAdapter		\scripts\

# Install
import and run Install-AdfsBankIDAdapter
Install-ADFSBankIDAdapter.ps1 -InstallDirectory "path to folder" -ConfigFile "path to settingsfile <BankIDSettings.json>"

# Uninstall
Uninstall-ADFSBankIDAdapter -InstallDirectory "path to folder"





