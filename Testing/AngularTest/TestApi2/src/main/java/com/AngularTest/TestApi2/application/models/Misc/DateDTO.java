package com.AngularTest.TestApi2.application.models.Misc;

import lombok.Data;
import lombok.NoArgsConstructor;
import java.time.LocalDate;
import java.time.LocalDateTime;
import lombok.AllArgsConstructor;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class DateDTO {
    private LocalDate Date;
    private LocalDateTime DateTime;

}