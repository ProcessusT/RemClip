<?php
	if(isset($_POST['hostname'])  && isset($_POST['data'])){
		try{
		    $db = new PDO('sqlite:./clipboard.db');
		    $db->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC);
		    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

			$hostname = strip_tags( preg_replace("/[^a-zA-Z0-9éèà=+!:,.; ][\n\r]+/", '',base64_decode($_POST['hostname'])));
			$message = strip_tags( preg_replace("/[^a-zA-Z0-9éèà=+!:,.; ][\n\r]+/", '', base64_decode($_POST['data'])));

			$req = $db->prepare('INSERT INTO clipboard (hostname, data, timedate) VALUES (?, ?, ?)');
		    $array_of_values = array($hostname, $message, date('Y-m-d H:i:s'));
			$req->execute($array_of_values);

		} catch(Exception $e) {
		    echo "Can't connect to database : ".$e->getMessage();
		    die();
			exit();
		}
	}else{
		echo "No data input";
		exit();
	}
?>