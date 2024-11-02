# cf-connection-messaging

Class library for implementing messaging between clients over a logical connection (E.g. UDP - Obviously not a 
connection). The purpose is to make it easy for client applications to send & receive messages without having to 
worry about the underlying protocol.

The client application's messages are converted to a generic message (ConnectionMessage) which is serialized,
transferred over the connection, deserialized and then passed to an event handler registered by the receiving
client. The receiving client then converts the ConnectionMessage back to the application specific message.

How to Send a Message
---------------------
- Client calls Connection.StartListening to enable receiving of messages.
- Client converts it's application specific message (E.g. ChatMessage) to a generic ConnectionMessage instance.
- Client calls Connection.Send(ConnectionMessage, EndpointInfo) which serializes the message and sends it
  via UDP.

How to Receive a Message
------------------------
- Client sets event handler Connection.OnConnectionMessage to handle received messages.
- Client calls Connection.StartListening to enable receiving of messages.
- Client receives packet(s) via UDP and deserializes them in to a ConnectionMessage instance.
- Client event handler Connection.OnConnectionMessage(ConnectionMessage) fires, ConnectionMessage instance is 
  converted to an application specific message (E.g. ChatMessage).
- Client processes application specific message.

Connection Class
----------------
The class handles the sending & receiving of messages (ConnectionMessage).

ConnectionMessage Class
-----------------------
The class stores the message in a generic format so that is can be deserialized and transferred over the connection.

The client application can create classes implementing the IExternalMessageConverter interface which will convert
between the application specific message (E.g. ChatMessage) and ConnectionMessage.

The main properties:
Id 				Unique Id
MessageTypeId	Application specific message type (E.g. "ChatMessage") so that receiver knows how to deserialize 
				the instance.
Parameters		Application specific message parameters.

Chat Sample Application
-----------------------
The sample application demonstrates use of the class library. The ChatConnection class makes use of the Connection
class internally to send & receive messages.