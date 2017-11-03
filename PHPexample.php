<?php

$secret='';
$key='';
$url="";
/*
array(
    'name' => $name,
    'email' => $email,
    'item'   => $item,
    'itemdescription' => $itemdescription,
    'amount' => $amount,
    'transactionid'   => $transactionid,
    'notes' => $notes
    
);
*/
$data ='name='.$name.'&email='.$email.'&item='.$item.'&amount='.$amount.'&transactionid='.$transactionid.'&itemdescription='.$itemdescription.'&notes='.$notes;


$nonce=time();
//$method="getaccount";
$method="newtransaction";
$post_data= $method.'&apikey='.$key.'&nonce='.$nonce;
$calculatedsign=hash_hmac('sha512', $post_data, $secret);
$s = curl_init();
curl_setopt($s,CURLOPT_URL,$url.$method);

curl_setopt($s,CURLOPT_HTTPHEADER,array('Key:'.$key,'Sign:'.$calculatedsign,'Nonce:'.$nonce)); 
curl_setopt($s, CURLOPT_POSTFIELDS, $data);
curl_setopt($s, CURLOPT_RETURNTRANSFER, 1);
curl_setopt($s, CURLOPT_FOLLOWLOCATION, 1);
curl_setopt($s, CURLOPT_POST, 1); 
//curl_setopt($s, CURLOPT_RETURNTRANSFER, true);
$response =curl_exec($s); 
//$httpcode = curl_getinfo($s, CURLINFO_HTTP_CODE);

echo $response."~";
curl_close($s); 


?>
