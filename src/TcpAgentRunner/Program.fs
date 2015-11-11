open System.Net
open System.Net.Sockets

let agent = new MailboxProcessor<string>(fun inbox ->
    printfn "bout to loop..."
    let rec loop() = async {
        let! msg = inbox.Receive()
        match (msg.ToLower()) with
        | s when s = "do" -> printfn "DOOOOOO!!!"
        | _ -> printfn "Don't!"
        return! loop()
    }
    loop()
)

let ip = Dns.GetHostEntry("localhost").AddressList.[0]
let socket = new TcpListener(ip, 3505)


let rec getMessage (stream: NetworkStream) msg =
    let buffer = Array.zeroCreate<byte>(100)
    let numBytes = stream.Read(buffer, 0, buffer.Length)
    printfn "%i bytes read" numBytes
    let s = System.Text.Encoding.Default.GetString(buffer, 0, numBytes)
    let newMsg = msg + s
    match stream.DataAvailable with
    | true -> getMessage stream newMsg
    | false -> newMsg.Trim()
        

[<EntryPoint>]
let main argv = 
    // start agent
    agent.Start()

    // start socket
    socket.Start()

    // listen loop -- TODO: make async
    while true do
        let client = socket.AcceptTcpClient()
        printfn "Connected to client"

        let stream = client.GetStream()
        let msg = getMessage stream ""
        if msg <> "" then
            agent.Post(msg)
            
        client.Close() |> ignore
    
    System.Console.ReadLine() |> ignore
    0 // return an integer exit code
