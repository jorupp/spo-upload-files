Run Create-SelfSignedCertificate.ps1 like this to generate the cer/pfx:
.\Create-SelfSignedCertificate.ps1 -CommonName "Rupp PnP test" -StartDate 2022-07-10 -EndDate 2042-01-01

Run through the rest of https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread to create the app registration and link the certificate to it

Copy the appsettings.json to appsettings.Development.json and fill in your site/etc., including the path to the .pfx you generated above and it's password