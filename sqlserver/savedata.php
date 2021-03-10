<?php

	$con = mysqli_connect('localhost', 'root', '', 'unityaccess');

	//check if the connection happens
	if (mysqli_connect_errno()) 
	{
		echo "1: Connection failed"; // error code #1 meaning connection failed
		exit();
	}
	
	$username = $_POST["name"];
	$newscore = $_POST["score"];

	//double checks that there is only one user with this name
	$namecheckquery = "SELECT Username FROM players WHERE Username='"  . $username . "';";


	$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed"); //error code #2 meaning namecheck query failed

	if (mysqli_num_rows($namecheck) != 1)
	{
		echo "5: Either no user with name, or more than one"; //error code $5 - number of names matching != 1
		exit();
	}

	$updatequery = "UPDATE players SET Score = " . $newscore . " WHERE Username = '" . $username . "';";
	mysqli_query($con, $updatequery) or die("7: Save query failed"); //error code #7 - UPDATE query failed

	echo "0";

?>
