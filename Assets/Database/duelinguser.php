<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$validateRequest = $_POST['validateRequest'];

$conn = new mysqli($servername, $username, $password, $dbname);

$sql = "UPDATE lobby SET ready = 'queue', duel='0', host='0', life='100' WHERE id = ?;";
$stmt = $conn->prepare($sql);
$stmt->bind_param("i", $validateRequest);

if ($stmt->execute()) {
    echo "success006";
}

$stmt->close();
$conn->close();