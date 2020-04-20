@ECHO OFF

java -jar signapk.jar platform.x509.pem platform.pk8 ../JukeboxVR_Home.apk ../JukeboxVR_Home_Signed.apk

Echo Signing Complete 

adb install -r ../JukeboxVR_Home_Signed.apk
adb shell settings put global captive_portal_detection_enabled 0

Echo Finish

Pause

EXIT