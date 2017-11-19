# YLE Search App (Nopea soumen uutiset - Fast finnish news)
This is a simple Unity program created to get quickly search information on YLE, Finnish news. https://areena.yle.fi/tv

## How it works
The app uses YLE API (http://developer.yle.fi/tutorials.html), it contacts the YLE server using Unity Web Request.

The syntax and rule for the request is found in YLEHelper.cs file

The Response from the server is a json file, which then, will be parsed into a C# struct, found in YLE Response.

The the YLEConnector response for handling the Connection between the app and the server.

The UI Manager responsible for the front-end UI of the app. There are just two parts:

  - Search bar, which contains the search bar and the search button.
  - Scroll view, where the users will scroll through all of the response received from the server.
  - The scroll view and the YLE Connector will only get the first 10 results from the search. When the scroll views detects the user has scroll down and is nearly reached the final news, it will request
    - Each piece of information will be stored and display in the Base UI Element class, which is a simple tab displaying the Title of the news and the thumbnail (if exists).
    - Upon clicking on the UI Element, it will be expanded and show more information about the program like: Description, Creator (if exists) media type, Air Date and operating service.
    - There is also a button to view the content directly on YLE, which show full information. 

## Known problem:
  - The app will keeps scrolling until all the news are displayed. This is, a problem as more displaying title means more tris count and more draw call (1 drawcall for each new thumbnail). The problem can be fix by either loading a new page complete, or culling all the hiden tile. Culling (either hiding, destroy completely or just deactive) is enough. There seems to be no problem of running out of memory since I try testing scrolling on my 1gb ram phone, it reach 400+ results and it doesn't crash, only slow, so memory is not a problem.
  
