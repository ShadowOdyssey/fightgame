<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$id = $_POST['actualListener'];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    echo "Connection failed: " . $conn->connect_error;
} else  {
    $stmt = $conn->prepare("SELECT forward, backward, attack1, attack2, attack3, hit, health FROM lobby WHERE id = ?");
    $stmt->bind_param("i", $id);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows > 0) {
        while ($row = $result->fetch_assoc()) {
            $combinedString = $row['forward'] . " " . $row['backward'] . " " . $row['attack1'] . " " . $row['attack2'] . " " . $row['attack3'] . " " . $row['hit'] . " " . $row['health'];
            echo $combinedString . "<br>";
        }
    }

    $stmt->close();
}

$conn->close();
?>