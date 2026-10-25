# TelegramBot-Scheduler

## How it works
- A bot that currently only answers to my chat-id, when written to.
- Using available 'functions' to read and write local files that contain the schedule (currently plain text).
- File name should be day name => wednesday = wednesday.txt
- Current Format: ID|NAME|DESCRIPTION|TIME , ex: 1|Read|Read a book|18:15

## Program.cs:
 - handles api-calls and connecting everything

## Answer.cs:
 - used for informing about next x events

## Events.cs:
 - event info (id, name, description, Time)

## FileServics.cs:
 - handles saving/reading settings from files

## Example File:
1|Wake up|Wake up and get ready for work|05:45
2|Leave|Leave and go to bus station|06:10
3|Work|Start working|07:00
4|Lunch|Go get lunch|11:30
5|Work|Start working again|12:20
6|Coffee|Go get a coffee|15:00
7|Leave|Pack and leave work|16:00
8|Train|Take the train home|16:15
9|Walk|Walk home|16:28
10|Eat|Eat something|17:00
11|Shower|Shower and clean|17:30
13|Read|Read a book|18:15
14|Free|Free time|18:45
15|Sleep|Go get some sleep|22:00
