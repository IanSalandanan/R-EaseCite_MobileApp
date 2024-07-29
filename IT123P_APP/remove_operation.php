<?php 

include_once('crud_functions.php');

$tableN = $_GET['table'];
$remove = $_GET['remove'];
$remove1 = isset($_GET['remove1']) ? $_GET['remove1'] : null;

$crud = new CRUD($con);

try {
    if (empty($tableN) || empty($remove) ) 
    {
        throw new Exception("Invalid input.");
    }

    if ($tableN === 'subject' && is_null($remove1)) 
    {
        $query = "DELETE FROM subject WHERE subj_id = ?;";
        $crud->remove($query, $remove);
    } 
    else if ($tableN === 'question')
    {
        $query = "DELETE FROM question WHERE ques_name = ? AND subj_id = ?;";
        $crud->remove($query, $remove, $remove1);
    }
    else if ($tableN === 'section' && is_null($remove1))
    {
        $query = "DELETE FROM section WHERE sect_id = ?;";
        $crud->remove($query, $remove);
    }
    else if ($tableN === 'student')
    {
        $query = "DELETE FROM student WHERE stud_name = ? AND sect_id = ?;";
        $crud->remove($query, $remove, $remove1);
    }


} catch (Exception $e) {
    echo "Error: " . $e->getMessage();
}

$con->close();
?>
