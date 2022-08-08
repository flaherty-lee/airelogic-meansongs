# airelogic-meansongs

Instructions to run application:
The easiest way to run this application is to open the solution in visual studio 2022 (or any version that supports .Net 6).
This will open a console in which the first console output will be a prompt to type in the artists name.

Once running a progress will be outputted based on the number of api calls being made. This will likely stay at 50% for a number of seconds as that will indicate all calls are being made but none have completed yet.

Once the application has completed you will have an opportunity to run again by typing Y or Yes to the question, or you can exit out the program.

Notes on slow performance:
Currently the application does not perform to a standard I am happy with, the main reason for this is the lyrics.ovh api. This api will take 20-30 seconds to make a single request and sometimes is quite temperamental in that it will fail due to a socket or https exception.
To mitigate these problems i make sure to run all apis calls as async tasks, also i have implemented a retry mechanism in LyricsovhService.cs.
If i was to do this task again i would choose another api to get the lyrics for each song/artist.