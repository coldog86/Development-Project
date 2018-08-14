<?php

$firebaseServerKey = "AAAA0zktg5E:APA91bHD6Txbg7nGuaEPUfUq3j10RerP9rvxnsfoc2FpwYqP8WgXZFdYtfBV0fxmo_OpgjZvZlP8JSdS_vBUk2Z28pRm_HOUGVC5ZzADLBGLwoW1KR_EbhUvWMADww1VEHYz8BMYGkq-6K_wP-RPAyHVSa-JLC1gnQ";

$firebaseUrl = "https://fcm.googleapis.com/fcm/send";

$notificationContent = [
	'title' => $_POST['title'],
	'body' => $_POST['body']
];

$notificationRecipient = $_POST['recipient'];

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