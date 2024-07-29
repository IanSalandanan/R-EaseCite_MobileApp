<?php 

include_once('crud_functions.php');

$tableN = $_GET['table'];
$new_update = $_GET['new_update'];
$orig_update = $_GET['orig_update']; 
$supp_update =  isset($_GET['supp_update']) ? $_GET['supp_update'] : null;  

$crud = new CRUD($con);

try {
    if (empty($tableN) || empty($new_update) || empty($orig_update)) {
        throw new Exception("Invalid input.");
    }

    if ($tableN === 'subject' && is_null($supp_update)) 
    {
        $query = "UPDATE subject SET subj_id = ? WHERE subj_id = ?;";
        $crud->update($query, $new_update, $orig_update);
    } 
    else if ($tableN === 'question')
    {
        $query = "UPDATE question SET ques_name = ? WHERE ques_name = ? AND subj_id = ?;";
        $crud->update($query, $new_update, $orig_update, $supp_update);
    }
    else if ($tableN === 'section'  && is_null($supp_update))
    {
        $query = "UPDATE section SET sect_id = ? WHERE sect_id = ?;";
        $crud->update($query, $new_update, $orig_update);
    }
    else if ($tableN === 'student')
    {
        $query = "UPDATE student SET stud_name = ? WHERE stud_name = ? AND sect_id = ?;";
        $crud->update($query, $new_update, $orig_update, $supp_update);
    }

} catch (Exception $e) {
    echo "Error: " . $e->getMessage();
}

$con->close();
?>
