import java.io.*;
import java.net.*;
import java.util.Arrays;
import java.util.Objects;


public class ServerThread extends Thread {
    private final Socket socket;

    public ServerThread(Socket socket) {
        super();
        this.socket = socket;
    }

    public void run() {
        try {
            BufferedReader in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
            PrintWriter out = new PrintWriter(socket.getOutputStream(), true);

            String line;
            while((line = in.readLine()) != null && !line.isEmpty())
            {
                String result;
                String[] request = line.split(" ");
                System.out.println("TCP got: " + line);
                if (Arrays.asList(Helper.operations).contains(request[0])) {
                    if (Objects.equals(request[0], Helper.operations[0])) {
                        try {
                            result = Integer.toString(Integer.parseInt(request[1]) + Integer.parseInt(request[2]));
                            Helper.requestedOperation[0]++;
                            Helper.statistics[3] += Integer.parseInt(result);
                        }
                        catch(Exception e) {
                            result = "Error";
                            Helper.statistics[2]++;
                        }
                    } else if (Objects.equals(request[0], Helper.operations[1])) {
                        try {
                            result = Integer.toString(Integer.parseInt(request[1]) - Integer.parseInt(request[2]));
                            Helper.requestedOperation[1]++;
                            Helper.statistics[3] += Integer.parseInt(result);
                        }
                        catch(Exception e) {
                            result = "Error";
                            Helper.statistics[2]++;
                        }
                    }
                    else if (Objects.equals(request[0], Helper.operations[2])) {
                        try {
                            result = Integer.toString(Integer.parseInt(request[1]) * Integer.parseInt(request[2]));
                            Helper.requestedOperation[2]++;
                            Helper.statistics[3] += Integer.parseInt(result);
                        }
                        catch(Exception e) {
                            result = "Error";
                            Helper.statistics[2]++;
                        }
                    }
                    else {
                        try {
                            result = Integer.toString(Integer.parseInt(request[1]) / Integer.parseInt(request[2]));
                            Helper.requestedOperation[3]++;
                            Helper.statistics[3] += Integer.parseInt(result);
                        }
                        catch(Exception e) {
                            result = "Error";
                            Helper.statistics[2]++;
                        }
                    }
                    out.write(result + "\r\n");
                    out.flush();
                    Helper.statistics[1]++;
                    System.out.println("TCP sent: " + result);
                }
                else {
                    out.write("Unknown command: " + request[0] + "\r\n");
                    out.flush();
                    System.out.println("Unknown command: " + request[0]);
                }
            }

        } catch (IOException e1) {
            // do nothing
        }
        finally {
            try {
                socket.close();
            } catch (IOException e) {
                System.err.println("Error closing socket: " + e.getMessage());
            }
        }
    }
}
