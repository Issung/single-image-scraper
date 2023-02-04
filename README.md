# single-image-scraper
Simple .NET console app to routinely download a singular URL to desired path with desired interval.

Designed to: 
* Not break during network issues (catch exception and log any error for each scrape loop).
* Work nicely across opens & closes (append file number by looking at current number of files in directory).
* End processing neatly (use CTRL + C to invoke tidy cancellation).

```
-o, --outputpath      Directory to output files to. Default is the application running directory.
-i, --inputurl        Required. URL to routinely scrape from.
-t, --timeinterval    Interval between scrapes, format HH:MM:SS. Default 1 minute.
--help                Display this help screen.
--version             Display version information.
```

Once I had all the images I wanted I used this command within Ubuntu WSL
```
ffmpeg -framerate 55 -i 'Image (%d).jpg' -vcodec h264 -crf 22 -s 3648x2052 "Timelapse.mp4"
```