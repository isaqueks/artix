# artix
Article Renderer to HTML   
### Running:
```dotnet ArtixPost.dll article.tix rules.xml outputArticle.html```

# Demo input file:
```
<?xml version="1.0" encoding="utf-8" ?>

<document>

  <HelloWorld />
  <link url="https://github.com/isaqueks/artix">
    GitHub repo
  </link>

</document>
```

# Demo rules file:
```
<?xml version="1.0" encoding="utf-8" ?>

<elements>

  <HelloWorld>
    <h1>Hello World!</h1>
  </HelloWorld>

  <link>
    <a href="$url">$content@trim</a>
  </link>

  <document>
    <html>
      <head>
        <title>Hello World - Artix</title>
      </head>
      <body>
        $content
      </body>
    </html>
  </document>
  
</elements>
```

# outputArticle.html
```
<html>

<head>
    <title>Hello World - Artix</title>
</head>

<body>
    <h1>Hello World!</h1>
    <a href="https://github.com/isaqueks/artix">GitHub repo</a>
</body>

</html>
```

<hr>

### Values
Values should start with `$` symbol.   
#### Predefined values:
`$content`: The rendered content    
`$rawcontent`: The raw inner text of the XML Element       
Other values are the element's attributes.   

##### Functions:
Functions can be called with value as parameter.   
#### Predefined functions:
`trim`: Trims a string    
`tolower`: Lower case a string    
`toupper`: Upper case a string    
`escapenewline`: Converts all the `\n` characters to html `<br />`    
`fsread`: Reads a file    

#### Calling a function:
The syntax is: `$value@function` or `$value@function1@function2` for a function chain.    

## Note:
Both input and rules file must be a valid XML!
