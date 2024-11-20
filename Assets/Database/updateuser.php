<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$collumn = $_POST['desiredCollumn'];
$value = $_POST['newValue'];
$referenceCollumn = $_POST['referenceCollumn'];
$validateCollumn = $_POST['validateCollumn'];

$conn = new mysqli($servername, $username, $password, $dbname);

$sql = "UPDATE lobby SET ? = ? WHERE ? = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ssss", $collumn, $value, $referenceCollumn, $validateCollumn);

if ($stmt->execute()) {
    echo "success004";
} else {
    echo "erro005";
}

$stmt->close();

$conn->close();
?>