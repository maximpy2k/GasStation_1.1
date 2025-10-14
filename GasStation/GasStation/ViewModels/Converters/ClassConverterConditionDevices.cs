namespace GasStation.ViewModels.Converters
{
    class ClassConverterConditionDevices 
    {
        public object Convert(object value,  object parameter,string type="")
        {
            var conv = "";

            switch ((string)value)
            {
                case "rrg":
                    conv = @"РРГ";
                    break;
                case "flap":
                    conv = @"Клапан";
                    break;
                case "vacuum":
                    conv = @"Вакууметр";
                    break;
                case "water":
                    conv = @"Д воды на охлаждение";

                    return $"{conv} {type} {parameter}";
                case "waterForward":
                    conv = @"Вода передний фланец";

                    return $"{conv} {type} {parameter}";
                case "waterBackward":
                    conv = @"Вода задний фланец";

                    return $"{conv} {type} {parameter}";
                case "fire":
                    conv = @"Д пламени";
                    break;
                //case "chamber":
                //    return "Термокамера";
                //case "burner":
                //    return "Горелка";
                case "dumperOpen":
                    conv = @"Заслонка открыта";
                    return $"{conv}";
                case "dumperClosed":
                    conv = @"Заслонка закрыта";
                    return $"{conv}";
                case "loadComplete":
                    conv = @"Загружен ОК";
                    return $"{conv}";
                case "unLoadComplete":
                    conv = @"Выгружен ОК";
                    return $"{conv} ";

                case "gateOpen":
                    conv = @"Затвор открыт";
                    return $"{conv} ";

                case "gateClosed":
                    conv = @"Затвор закрыт";
                    return $"{conv} ";
                    //case "chamber":
                    //    return "Термокамера";
                    //case "burner":
                    //    return "Горелка";
            }

            if ((string)value == @"section")
            {
                switch ((int)parameter)
                {
                    case 1:
                        conv = @"Термосекция(Газ)";
                        break;

                    case 2:
                        conv = @"Термосекция(Центр)";
                        break;

                    case 3:
                        conv = @"Термосекция(Загр.)";
                        break;
                }
            }

            if ((string)value == @"td")
            {
                switch ((int)parameter)
                {
                    case 1:
                        conv = @"Т горелки";
                        break;

                    case 2:
                        conv = @"Т пламени";
                        break;
                }
            }

            if ((string)value == @"dio")
            {
                switch ((int)parameter)
                {
                    case 1:
                        conv = @"Заслонка открыта";
                        break;

                    case 2:
                        conv = @"Заслонка закрыта";
                        break;

                    case 3:
                        conv = @"Загружен ОК";
                        break;

                    case 4:
                        conv = @"Выгружен ОК";
                        break;
                }

                return $"{conv}";
            }

            return $"{conv} {parameter}{type}";
        }
    }
}
