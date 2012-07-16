package com.seriouscatgames.couchlover.client;

import android.app.Activity;
import android.os.Bundle;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Toast;
import com.seriouscatgames.couchlover.client.net.*;

public class CouchLoverClientActivity extends Activity {
    /** Called when the activity is first created. */
	
	public TcpClient client;
	
	public int startX, startY;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        
        startX = 0;
        startY = 0;
        
		try {
			client = new TcpClient("192.168.178.39", 666);
			client.Connect();
			Toast.makeText(getApplicationContext(), "Logged on", Toast.LENGTH_LONG).show();
		} catch(Exception e) {
			Toast.makeText(getApplicationContext(), e.getMessage(), Toast.LENGTH_LONG).show();
		}
    }
    
    @Override
    public boolean onTouchEvent(MotionEvent event) {
        int x = (int)event.getX();
        int y = (int)event.getY();
        switch (event.getAction()) {
            case MotionEvent.ACTION_DOWN:
            	startX = x;
            	startY = y;
            	break;
            case MotionEvent.ACTION_MOVE:
            	int deltaX = x - startX;
            	int deltaY = y - startY;
            	client.SendMessage("mouseMove:"+ deltaX + ";" + deltaY);
            	
            	startX = x;
            	startY = y;
            	
            	break;
            case MotionEvent.ACTION_UP:
            	client.SendMessage("mouseUp:"+ x + ";" + y);
            	break;
        }
        return false;
    }

    public void rightClick(View view) {
        client.SendMessage("rightClick:");
    }
    
    public void leftClick(View view) {
    	client.SendMessage("leftClick:"); 
    }    
    
    
}