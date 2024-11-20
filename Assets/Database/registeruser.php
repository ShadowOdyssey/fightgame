<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$name = $_POST['currentName'];

$forward = 'no';
$backward = 'no';
$attack1 = 'no';
$attack2 = 'no';
$attack3 = 'no';
$hit = 'no';
$health ='no'; 
$wins = 0;
$profile = 0;
$ready = 'no';
$status = 'online';

$conn = new mysqli($servername, $username, $password, $dbname);

$stmt = $conn->prepare("INSERT INTO lobby(name, forward, backward, attack1, attack2, attack3, hit, health, wins, profile, ready, status) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

$stmt->bind_param("ssssssssiiss", $name, $forward, $backward, $attack1, $attack2, $attack3, $hit, $health, $wins, $profile, $ready, $status);

if ($stmt->execute()) {
    echo "success002";
} else {
    echo "error003";
}

$stmt->close();
$conn->close();