import java.io.IOException;
import java.net.*;
import java.net.InetAddress;
import java.util.ArrayList;

public class DAS {
    ArrayList<Integer> store = new ArrayList<Integer>();
    private DatagramSocket server;
    private static boolean isMaster = true;
    private static boolean serverListen = true;
    static private int masterPort;
    static private int port;
    static private int number;

    public DAS() throws SocketException{
        if (!isMaster) {
            port = Helper.getRandomPort(5);
        }
        initializeServer();
    }

    private void initializeServer() throws SocketException {
        server = new DatagramSocket(port);
        System.out.println("App running");
        if (isMaster) {
            System.out.println("Mode: Master");
        }
        else {
            System.out.println("Mode: Slave");
        }
        System.out.println("Port: " + server.getLocalPort());
        System.out.println("Number: " + number);
        System.out.println(" ");

    }

    private void service() throws IOException {
        InetAddress address = InetAddress.getLocalHost();
        if (isMaster) {
            byte[] buff = new byte[UDP.MAX_DATAGRAM_SIZE];
            final DatagramPacket datagram = new DatagramPacket(buff, buff.length);

            server.receive(datagram);

            new Thread(() -> {
                int n = 0;
                boolean received = false;
                try {
                    n = Integer.parseInt(new String(datagram.getData(), 0, datagram.getLength()));
                    received = true;
                }
                catch(Exception e) {
                    System.out.println("Received message: " + new String(datagram.getData(), 0, datagram.getLength()));
                }
                if (received) {
                    if (n == 0) {
                        store.add(number);
                        int average = Helper.average(store);
                        store.remove(store.size() - 1);
                        System.out.println("Average: " + average);
                        try {
                            String response = "Broadcasting average " + average;
                            byte[] respBuff = response.getBytes();
                            DatagramPacket resp = new DatagramPacket(respBuff, respBuff.length, address, port);
                            server.send(resp);
                            System.out.println("I've sent " + response);
                        } catch (IOException e) {
                            System.out.println("Average error");
                        }
                    }
                    else if (n == -1) {
                        System.out.println("Received: " + n);
                        try {
                            String response = "Broadcasting number " + n;
                            byte[] respBuff = response.getBytes();
                            DatagramPacket resp = new DatagramPacket(respBuff, respBuff.length, address, port);
                            server.send(resp);
                            System.out.println("I've sent " + response);
                            System.out.println("Exit");
                            server.close();
                            serverListen = false;
                        } catch (IOException e) {
                            System.out.println("Error");
                        }
                    }
                    else {
                        System.out.println("Received: " + n);
                        store.add(n);
                    }

                }
            }).start();
        }

        else {
            byte[] queryBuff = String.valueOf(number).getBytes();
            DatagramPacket query = new DatagramPacket(queryBuff, queryBuff.length, address, masterPort);

            server.send(query);
            System.out.println("Number sent: "  + number);
            System.out.println("Exit");
            server.close();
            serverListen = false;
        }
    }

    public void listen() {
        while(serverListen) {
            try {
                service();
            } catch (IOException e) {
                System.out.println("Listen error");
            }
        }
    }

    public static void main(String[] args) throws SocketException {
        if (args.length != 2) {
            System.out.println("Usage: java DAS <port> <number>");
            return;
        }

        try {
            masterPort = Integer.parseInt(args[0]);
            port = Integer.parseInt(args[0]);
            number = Integer.parseInt(args[1]);
        }
        catch (Exception e){
            System.out.println("Provide a valid integer for <port> and <number>");
            return;
        }

        try {
            isMaster = true;
            new DAS().listen();
        }
        catch (BindException e) {
            System.out.println("Port " + port + " is already in use");
            try {
                isMaster = false;
                new DAS().listen();
            } catch (SocketException err) {
                System.out.println("Error");
            }
        }
    }
}
