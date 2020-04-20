@ECHO OFF

java -jar signapk.jar platform.x509.pem platform.pk8 unsigned.apk signed.apk

Echo Signing Complete 

 
Pause
EXIT