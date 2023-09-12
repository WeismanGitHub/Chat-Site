//import * as signalR from "@microsoft/signalr";
//import "./css/main.css";

//const divMessages: HTMLDivElement = document.querySelector("#divMessages")!;
//const tbMessage: HTMLInputElement = document.querySelector("#tbMessage")!;
//const btnSend: HTMLButtonElement = document.querySelector("#btnSend")!;
//const username = new Date().getTime();

//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/hub")
//    .build();

//connection.on("messageReceived", (username: string, message: string) => {
//    const m = document.createElement("div");

//    m.innerHTML = `<div class="message-author">${username}</div><div>${message}</div>`;

//    divMessages.appendChild(m);
//    divMessages.scrollTop = divMessages.scrollHeight;
//});

//connection.start().catch((err: Error) => document.write(String(err)));

//tbMessage.addEventListener("keyup", (e: KeyboardEvent) => {
//    if (e.key === "Enter") {
//        send();
//    }
//});

//btnSend.addEventListener("click", send);

//function send() {
//    connection.send("newMessage", username, tbMessage.value)
//        .then(() => (tbMessage.value = ""));
//}

import { useEffect, useState } from 'react';
const App = () => {
    const [final, setFinal] = useState<{ summary: string }[]>([]);

    useEffect(() => {
        fetch('http://192.168.1.45:8083/WeatherForecast').then((resp) => resp.json()).then((data) => { setFinal(data) });
    }, [])

    console.log(final);
    return (
        <>
            <ul>
                {
                    final.map((data) => {
                        return <li key={ data.summary }>{ data.summary }</li>
                    })
                }
            </ul>
        </>
    )
}


export default App;
