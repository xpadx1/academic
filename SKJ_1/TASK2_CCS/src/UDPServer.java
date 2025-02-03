import java.io.IOException;
import java.net.*;
import java.util.Objects;

public class UDPServer {
    private DatagramSocket server;
    private final int serverPort;
    private static boolean serverListen = true;

    public UDPServer(int port) throws SocketException {
        serverPort = port;
        initializeServer();
    }

    private void initializeServer() throws SocketException {
        server = new DatagramSocket(serverPort);
        System.out.println("UDP Server listens on port: " + server.getLocalPort());
    }

    private void UDPservice() throws IOException {
        byte[] buff = new byte[Helper.MAX_DATAGRAM_SIZE];
        final DatagramPacket datagram = new DatagramPacket(buff, buff.length);

        server.receive(datagram);

        new Thread(() -> {
            String n = new String(datagram.getData(), 0, datagram.getLength());
            String[] request = n.split(" ");
            if (Objects.equals(request[0], "CCS") && Objects.equals(request[1], "DISCOVER")) {
                System.out.println("UDP got: " + n);
                String response = "CCS FOUND";
                try {
                    response = response + " " + InetAddress.getLocalHost().getHostAddress();
                } catch (UnknownHostException e) {
                    throw new RuntimeException(e);
                }
                byte[] respBuff = String.valueOf(response).getBytes();
                int clientPort = datagram.getPort();
                InetAddress clientAddress = datagram.getAddress();
                DatagramPacket resp = new DatagramPacket(respBuff, respBuff.length, clientAddress, clientPort);
                try {
                    server.send(resp);
                    System.out.println("UDP sent: " + response);
                } catch (IOException e) {
                    // do nothing
                }
            } else {
                System.out.println("Invalid message");
            }

        }).start();
    }

    public void listen() throws SocketException {
        while (serverListen) {
            try {
                UDPservice();
            } catch (IOException e) {
                System.out.println("Listen error");
            }
        }
    }
}
