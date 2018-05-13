<?php

//Variables for the connection
	$servername = "localhost";
	$server_username =  "u697839110_col";
	$server_password = "collin";
	$dbName = "u697839110_40k";
	
	$catagoryArray = array();
	$dataArray = array('catagory' => array("cat", array(), "quest"));
	$testArray = array();
	$finalArray = array();


	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$sql = "SELECT DISTINCT catagory		
				FROM q2;";
	$result = mysqli_query($conn, $sql);
	
		
	if(mysqli_num_rows($result) > 0)
	{		
		while($row = mysqli_fetch_assoc($result))
		{
			$row_array = $row['catagory'];
			//push the values in the array
			array_push($catagoryArray,$row_array);
        }		
	}
	
//for each catagory get every associated question
for($i = 0; $i<sizeof($catagoryArray); $i++)
{	
	$questionArray = array();
	$sql = "SELECT QuestionText FROM `q2` WHERE Catagory = \"" . $catagoryArray[$i] . "\";";
	$result = mysqli_query($conn, $sql);
		
	if(mysqli_num_rows($result) > 0)
	{		
		while($row = mysqli_fetch_assoc($result))
		{
			$row_array = $row['QuestionText'];
			//push the values in the array
            array_push($questionArray,$row_array);
        }		
	}
    
		for($n = 0; $n<sizeof($questionArray); $n++)
		{
		    $answerArray = array();
	    	$sql = "SELECT CorrectAns, WrongAns1, WrongAns2, WrongAns3 FROM `q2` WHERE QuestionText = \"" .             $questionArray[$n] . "\";"; 
	    	$result = mysqli_query($conn, $sql);
		    if(mysqli_num_rows($result) > 0)
        	{
    	    	while($row = mysqli_fetch_assoc($result))
    		    {
    		        $answerArray[] = array('answerText' => $row['CorrectAns'], 'isCorrect' => true);
                    $answerArray[] = array('answerText' => $row['WrongAns1'], 'isCorrect' => false);
                    $answerArray[] = array('answerText' => $row['WrongAns2'], 'isCorrect' => false);
                    $answerArray[] = array('answerText' => $row['WrongAns3'], 'isCorrect' => false);

    		        
    		    }
		        
        	}
        	echo $questionArray[$n]; echo "<br>";
			$testArray[] = array('questionText' => $questionArray[$n], 'answers' => array($answerArray));
		}
		$finalArray[] = array('catagory' => $catagoryArray[$i], 'questions' => array($testArray));
		$testArray = array();
		
		
}	
	
    echo json_encode($finalArray);
?>
