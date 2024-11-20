<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$selection = $_POST['desiredSelection'];
$table = $_POST['currentTable'];
$collumn = $_POST['currentCollumn'];
$search = $_POST['newSearch'];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error)
 {
    echo "erro001";
}
else
{
    $sql = "SELECT " . $selection . " FROM " . $table . " WHERE " . $collumn . " = " . $search;

    $result = $conn->query($sql);

    if ($result->num_rows > 0)
    {
        while($row = $result->fetch_assoc())
        {
            echo $row[$selection];
        }
    }
    else
    {
        echo "error002";
    }
}

$conn->close();
?>