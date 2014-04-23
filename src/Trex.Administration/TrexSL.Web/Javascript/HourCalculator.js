    function CalcDecimalHours(sourceId,targetId)
    {
    
            var source = document.getElementById(sourceId);
        
        if(!source)
            return;
            
       var sourceHourValue = source.value;
    
    
       var hour = sourceHourValue.split(":")[0];
       hour = hour.replace("_","");
       
       if(isNaN(hour))
            hour = 0;
       
       var minutes = sourceHourValue.split(":")[1];
       if(minutes)
            minutes = minutes.replace("_","");
       
       
       var decimalMinutes = 0;

       if(!isNaN(minutes))       
        minutes =minutes /60;
        else
        minutes = 0;
       var decimalHours = eval(hour) + eval(minutes);
       decimalHours =decimalHours.toFixed(2);
       
       var target = document.getElementById(targetId);
       
       if(target)
       {
        target.value = decimalHours.replace(".",",");
       }
       
    }