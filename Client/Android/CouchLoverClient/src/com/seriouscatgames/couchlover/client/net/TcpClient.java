package com.seriouscatgames.couchlover.client.net;

import java.io.*;
import java.net.InetAddress;
import java.net.Socket;
import android.util.Log;
 
public class TcpClient implements Runnable {
	private String pServerIp;
	private int pServerPort;

	private Socket socket;
	private BufferedWriter bw;
	private BufferedReader br; 
	
	
	public TcpClient(String aServerIp, int aServerPort)
	{
		pServerIp = aServerIp;
		pServerPort = aServerPort;
	}

	public void Connect()
	{
		try
		{                   
             InetAddress serverAddr = InetAddress.getByName(pServerIp);            
             Log.d("TCP", "C: Connecting...");
             socket = new Socket(serverAddr, pServerPort);
             try 
             {
            	 Log.e("TCP", "Connecting...");
                 OutputStream socketoutstr = socket.getOutputStream(); 
                 OutputStreamWriter osr = new OutputStreamWriter( socketoutstr ); 
                 bw = new BufferedWriter( osr ); 

                 InputStream socketinstr = socket.getInputStream(); 
                 InputStreamReader isr = new InputStreamReader( socketinstr ); 
                 br = new BufferedReader( isr ); 
            	 
            	 Log.e("TCP", "Connected...");            	             	
             } 
             catch(Exception e) 
             {
            	 Log.e("TCP", "S: Error", e);
            	 socket.close();
             } 
		} 
		catch (Exception e) 
		{
			Log.e("TCP", "C: Error", e);
		}		
	}

	public void run()
	{ 	
	}
	
	private void ReadMessage(String message)
	{
		
		
	}

	public void SendMessage(String message)
	{
		try
		{
			bw.write(message);
			bw.newLine();
			bw.flush();
		} 
		catch (IOException e) 
		{
			e.printStackTrace();
		}	
	}

	public void Close() 
	{
		if(socket != null) 
		{
			try 
			{
				socket.close();
			} 
			catch (Exception e) 
			{
				Log.e("TCP", "C: Error",e);
			}			
		}		
	}
}    
