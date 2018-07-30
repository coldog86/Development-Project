<?php

require_once("serverInformation.php");

$firebaseServerKey = $googleServerKey;

$firebaseUrl = "https://fcm.googleapis.com/fcm/send";

$notificationContent = [
	'title' => "[Notification Title Placeholder]",
	'body' => "[Notification Body Placeholder]"
];

$notificationRecipient = "/topics/all";

$notificationData = [
	'powerLevel' => "9001",
	'dataString' => "[Data String Placeholder]"
];

$notificationFields = [
	'to' => $notificationRecipient,
	'notification' => $notificationContent,
	'data' => $notificationData
];

$firebaseHeaders =  [
	'Authorization: key='.$firebaseServerKey,
    'Content-Type: application/json'	
];

$curl = curl_init();

curl_setopt($curl, CURLOPT_URL, $firebaseUrl);
curl_setopt($curl, CURLOPT_POST, true);
curl_setopt($curl, CURLOPT_HTTPHEADER, $firebaseHeaders);
curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
curl_setopt($curl, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($curl, CURLOPT_POSTFIELDS, json_encode($notificationFields));

$result = curl_exec($curl);

curl_close($curl);

echo $result;
?>