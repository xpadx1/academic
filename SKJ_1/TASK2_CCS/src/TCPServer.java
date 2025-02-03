import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

public class TCPServer {
    int serverPort;

    public TCPServer(int port) {
        serverPort = port;
    }

    public void listenSocket() {
        ServerSocket server = null;
        Socket client = null;

        try {
            server = new ServerSocket(serverPort);
        }
        catch (IOException e) {
            System.out.println("Could not listen");
            System.exit(-1);
        }
        System.out.println("TCP Server listens on port: " + server.getLocalPort());

        while(true) {
            try {
                client = server.accept();
                Helper.statistics[0]++;
            }
            catch (IOException e) {
                System.out.println("Accept failed");
                System.exit(-1);
            }

            (new ServerThread(client)).start();
        }

    }
}
