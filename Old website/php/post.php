<?php  
 function died($error) {
 
        // your error code can go here
 
        echo "Sorry, er is een probleem opgetreden bij u verzenden van je formulier.";
 
        echo "De fouten verschijnen hier onder.<br /><br />";
 
        echo $error."<br /><br />";
 
        echo "Ga alstublieft terug en pas de fouten aan.<br /><br />";
 
        die();
 
    }
 
     
 
    // validation expected data exists
 
    if(!isset($_POST['naam']) ||
 
        !isset($_POST['email']) ||
 
        !isset($_POST['telefoon']) ||
 
        !isset($_POST['bericht'])) {
 
        died("Sorry, er is een probleem opgetreden bij u verzenden van je formulier.");       
 
    }
 
     
 
    $naam = $_POST['naam']; // required
 
    $email = $_POST['email']; // required
 
    $telefoon = $_POST['telefoon']; // not required
 
    $bericht = $_POST['bericht']; // required
 
     
 
    $error_message = "";
 
    $email_exp = '/^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/';
 
  if(!preg_match($email_exp,$email)) {
 
    $error_message .= 'Het e-mailadres dat je hebt ingegeven is niet correct.<br />';
 
  }
 
    $string_exp = "/^[A-Za-z .'-]+$/";
 
  if(!preg_match($string_exp,$naam)) {
 
    $error_message .= 'De naam die je hebt ingegeven bestaat uit tekens die niet geldig.<br />';
 
  }

 
  if(strlen($bericht) == 0) {
 
    $error_message .= '<br />Er moet een bericht ingegeven zijn.<br />';
 
  }
 
  if(strlen($error_message) > 0) {
 
    died($error_message);
 
  }
 
     
 
    function clean_string($string) {
 
      $bad = array("content-type","bcc:","to:","cc:","href");
 
      return str_replace($bad,"",$string);
 
    }
  
$naam = htmlspecialchars($_POST['naam']);  
$email = htmlspecialchars($_POST['email']);
$telefoon = htmlspecialchars($_POST['telefoon']);
$bericht = htmlspecialchars($_POST['bericht']);  
  
$tijd = time();  
$datum = strftime('%d/%m/%y %H:%M', $tijd);

if (empty($telefoon))
{
  $message = $naam.' met het e-mailadres '.$email.' stuurde op '.$datum.' het volgende bericht:  

'.$bericht.''; 
} else
{
  $message = $naam.' met het e-mailadres '.$email. ' en telefoon ' .$telefoon. ' stuurde op '.$datum.' het volgende bericht:  

'.$bericht.''; 
}
  
mail('kunstenboetiek@hotmail.com', 'kunstenboetiek.be - contact', $message, 'From: '.$email);
  
echo 'Uw bericht is verzonden. U krijgt zo snel mogelijk antwoord.';  
?> 