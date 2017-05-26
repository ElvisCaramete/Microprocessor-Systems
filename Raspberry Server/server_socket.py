import socket

backlog=1
size = 1024
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind(("172.16.254.90",50001))
s.listen(backlog)
try:
    print ( "is waiting")
    client, address = s.accept()

    while 1:
        data = client.recv(size)
        if data:
            tabela = data
			# tabela is the data received from mobile application
            print ("data received")
            print (tabela)
			
		   # Here we send data to mobile application
           # client.send(data)
except:
    print("closing socket")
    client.close()
    s.close()
