
<?php

//Variables for the connection
	$servername = "localhost";
	$server_username =  "u697839110_col";
	$server_password = "collin";
	$dbName = "u697839110_40k";
	
	$gameNumbersString = $_POST["gameNumbersPost"];
	//$gameNumbersString = "55194,26117,37969,39617";
    //$gameNumbersString = "33632";
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$array = array();
    $num = 0;
	$arrayOfGameNumbers = explode(",", $gameNumbersString);
	foreach ($arrayOfGameNumbers as $value)
	{
		$sql = "SELECT * FROM `OngoingGames` where gameNumber = $value";
		//echo "$sql <br>";
		$result = mysqli_query($conn, $sql);
		
		if(mysqli_num_rows($result) > 0)
		{
			
				while($row = mysqli_fetch_assoc($result))
			{
				//$array[] = array('' => $row["totalScore"]);
				$array[$num] = $row;
				//$array[$result];
				$num++;
				
			}
			
		}
	}
    
    //echo '<pre>', print_r($array, true), '</pre>';
    $string = json_encode($array);
	echo $string;
?>
