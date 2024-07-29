<?php 

include_once('crud_functions.php');

$tableN = $_GET['table'];
$value = $_GET['insert'];
$value1 = isset($_GET['insert1']) ? $_GET['insert1'] : null;

$crud = new CRUD($con);

try {
    if (empty($tableN) || empty($value)) {
        throw new Exception("Invalid input.");
    }

    if (is_null($value1)) {
        $query = "INSERT INTO $tableN VALUES (?)";
        $crud->add($query, $value);

    } else {
        $query = "INSERT INTO $tableN VALUES (?, ?)";
        $crud->add($query, $value, $value1);
    }

} catch (Exception $e) {
    echo "Error: " . $e->getMessage();
}

$con->close();
?>
