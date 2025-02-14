Mohammad Hadi Maghami
Feb 2025 - Tehran

# Message Broker Project for C# Bootcamp

## 📂 Project Structure  
This project consists of **5 main folders**:
1. **MessageBroker**
2. **ProducerLibrary**
3. **ProducerApp**
4. **ConsumerLibrary**
5. **ConsumerApp**

---

## **📌 MessageBroker**
The Message Broker is an **ASP.NET Web API** that functions as an **intermediary** between a Producer and a Consumer, similar to **Kafka**.  
- It **receives messages (HTTP GET)** and **sends messages (HTTP POST)** via **HTTP requests**.  
- Messages are stored in a **JSON file** to ensure data persistence.  
- The **LoggerService.cs** file contains methods for logging **INFO, WARNING, and ERROR** messages.  
- The **messages.json** and **log.txt** files are stored in the `.\Storage\` directory.  
- The built-in **Swagger extension** is added for API testing and educational purposes.  

---

## **📌 ProducerLibrary**
The Producer Library defines the **general instructions** for interacting with the **Message Broker API**.  
- It contains **interfaces and attributes** for creating data.  
- It allows setting parameters like **Rate Limit** and **Retry Mechanisms**.  

---

## **📌 ProducerApp**
The **ProducerApp** is an **implementation** of the **Producer Library**.  
- It interacts with the **Message Broker API** to **send generated data**.  
- It **loads the methods dynamically** from `ProducerLibrary.dll` (located in the `Addons` folder) using **Reflection**.  

---

## **📌 ConsumerLibrary**
Similar to **ProducerLibrary**, the **ConsumerLibrary** defines **interfaces and attributes** for receiving data from the **Message Broker API**.  

---

## **📌 ConsumerApp**
The **ConsumerApp** is an **implementation** of the **Consumer Library**.  
- It interacts with the **Message Broker API** to **fetch messages** from the queue.  
- It **loads the methods dynamically** from `ConsumerLibrary.dll` (located in the `Addons` folder) using **Reflection**.  

---




//
*******************************************************
//
How to test the project:

1. Running the Web API

I. open the windows PowerShell terminal on MessageBroker directory:
.\MessageBroker\MessageBroker
II. Type -> dotnet run
III. Wait for the Web API to load


2. Running the ProducerApp

I. I. open the windows PowerShell terminal on ProducerApp directory:
.\ProducerApp\ProducerApp
II. Type -> dotnet run
III. Wait for the app to load
IV. Enter a name for the producer (unique ID) 
V. The Producer app should start sending data to the MessageBroker


3. Running the ConsumerApp

I. I. open the windows PowerShell terminal on ConsumerApp directory:
.\ConsumerApp\ConsumerApp
II. Type -> dotnet run
III. Wait for the app to load
IV. Enter the name for the producer you want to get data from (unique ID) 
V. The Consumer app should start receving data to the MessageBroker


//
//
/// You can Also run the RunAll.cmd to run MessageBroker and ProducerApp and ConsumerApp at the same time.
//
//


////////////////////////////////////////////

⚠️ Additional Notes
There is an Addons folder inside both the ProducerApp and ConsumerApp directories.
This folder contains the DLL files used for dynamic method loading via Reflection.
The Addons folder must also be placed in the debug directory (.\ConsumerApp\ConsumerApp\bin\Debug\net8.0) to ensure the applications can load the DLLs properly. (Not sure why this happens :) , but it ensures correct loading.)
Make sure that the localhost port 5052 is free to be able to run the web api (or change the port manually in the source code)


