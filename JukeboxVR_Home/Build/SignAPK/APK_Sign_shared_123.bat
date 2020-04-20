
@ECHO OFF
Echo Auto-sign Created By Dave Da illest 1 
Echo Update.zip is now being signed and will be renamed to update_signed.zip

java -jar signapk.jar shared.x509.pem shared.pk8 123.apk signedShared_123.apk

Echo Signing Complete 
 
Pause
EXIT