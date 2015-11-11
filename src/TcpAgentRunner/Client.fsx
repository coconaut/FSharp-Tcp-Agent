open System.Net
open System.Net.Sockets

let bytes = System.Text.Encoding.Default.GetBytes("do")
let client = new TcpClient("localhost", 3505)
let stream = client.GetStream()
stream.Write(bytes, 0, bytes.Length)

