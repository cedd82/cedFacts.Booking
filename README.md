# FACTS.Booking

to generate api docs i.e. the ApiDocument.htmnl read this: 
 
https://github.com/aubm/postmanerator

download the exe from the repo or use the one in this solution :

the command to generate the docs:
postmanerator -output=./doc.html -collection=./postmanCollection.json


steps to generate docs:

1. import swagger json into postman collection
2. export postman collection save as json on file system for the input flag in the cmd: -i 
3. run the exe  e.g.
	postmanerator -output=./doc.html -collection=./postmanCollection.json


postmanerator -output=./ApiDocumentation.html -collection=./x.json
