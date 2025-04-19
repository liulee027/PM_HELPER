using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace PMHelperService.CLASS
{
    public class PipeClient
    {
        private const string PIPE_NAME = "PMServicePipe";
        private NamedPipeClientStream pipeClient;
        private StreamReader reader;
        private StreamWriter writer;
        private CancellationTokenSource cts;

        public event Action<string> MessageReceived;
        public event Action<string> StatusChanged;

        public void Start()
        {
            pipeClient = new NamedPipeClientStream(PIPE_NAME, "Client", PipeDirection.InOut, PipeOptions.Asynchronous);
            cts = new CancellationTokenSource();

            Task.Run(() => ConnectToServer(cts.Token));
        }

        private async Task ConnectToServer(CancellationToken token)
        {
            try
            {
                await pipeClient.ConnectAsync(token);
                reader = new StreamReader(pipeClient);
                writer = new StreamWriter(pipeClient) { AutoFlush = true };

                StatusChanged?.Invoke("Connected to server");

                Task.Run(() => ReceiveMessages(token));
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke($"Connection error: {ex.Message}");
            }
        }

        private async Task ReceiveMessages(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    string message = await reader.ReadLineAsync();
                    if (message != null)
                    {
                        MessageReceived?.Invoke(message);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke($"Receive error: {ex.Message}");
            }
        }

        public async Task SendMessage(string message)
        {
            try
            {
                if (pipeClient.IsConnected)
                {
                    await writer.WriteLineAsync(message);
                }
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke($"Send error: {ex.Message}");
            }
        }

        public void Stop()
        {
            cts?.Cancel();
            pipeClient?.Close();
            pipeClient?.Dispose();
            StatusChanged?.Invoke("Disconnected from server");
        }
    }
}
