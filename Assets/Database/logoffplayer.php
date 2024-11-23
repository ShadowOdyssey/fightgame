<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$validateRequest = $_POST['validateRequest'];

$conn = new mysqli($servername, $username, $password, $dbname);

$sql = "UPDATE lobby SET forward='no', backward='no', attack1='no', attack2='no', attack3='no', hit='no', health='100', profile='0', ready='no', status='offline' duel='0', host='0' WHERE id = ?;";
$stmt = $conn->prepare($sql);
$stmt->bind_param("i", $validateRequest);

if ($stmt->execute()) {
    echo "success006";
}

$stmt->close();
$conn->close();